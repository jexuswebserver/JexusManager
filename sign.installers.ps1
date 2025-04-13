if ($env:CI -eq "true") {
    exit 0
}

# Function to check if a binary is ARM64
function Test-IsArm64Binary {
    param (
        [Parameter(Mandatory = $true)]
        [string]$FilePath
    )

    try {
        # Read the PE header to determine the architecture
        $fileStream = [System.IO.File]::OpenRead($FilePath)
        $binaryReader = New-Object System.IO.BinaryReader($fileStream)
        
        # Check DOS header magic number (MZ)
        $dosHeader = $binaryReader.ReadUInt16()
        if ($dosHeader -ne 0x5A4D) { # "MZ" signature
            $binaryReader.Close()
            $fileStream.Close()
            return $false
        }
        
        # Seek to e_lfanew field (offset 60)
        $fileStream.Position = 60
        $peHeaderOffset = $binaryReader.ReadUInt32()
        
        # Go to PE header and verify signature
        $fileStream.Position = $peHeaderOffset
        $peSignature = $binaryReader.ReadUInt32()
        if ($peSignature -ne 0x00004550) { # "PE\0\0" signature
            $binaryReader.Close()
            $fileStream.Close()
            return $false
        }
        
        # Machine type is at offset PE header + 4
        $fileStream.Position = $peHeaderOffset + 4
        $machineType = $binaryReader.ReadUInt16()
        
        $binaryReader.Close()
        $fileStream.Close()
        
        # 0x8664 = AMD64 (x64), 0x14c = i386, 0xAA64 = ARM64
        return $machineType -eq 0xAA64
    }
    catch {
        Write-Host "Error examining binary: $_"
        return $false
    }
}

$cert = Get-ChildItem -Path Cert:\CurrentUser\My -CodeSigningCert | Select-Object -First 1
if ($null -eq $cert) {
    Write-Host "No code signing certificate found in MY store. Exit."
    exit 1
}

Write-Host "Certificate found. Sign the MSI installers."

# Determine if we're running on ARM64
$isArm64System = [System.Environment]::Is64BitOperatingSystem -and [System.Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture -eq [System.Runtime.InteropServices.Architecture]::Arm64

# Find signtool.exe candidates
$signtoolCandidates = Get-ChildItem -Path "${env:ProgramFiles(x86)}\Windows Kits\10\bin" -Recurse -Filter "signtool.exe"
if ($signtoolCandidates.Count -eq 0) {
    Write-Host "No signtool.exe found. Exit."
    exit 1
}

# Prioritize ARM64 candidates if on ARM64 system
if ($isArm64System) {
    Write-Host "Running on ARM64 Windows, prioritizing ARM64 signtool..."
    $prioritizedCandidates = @()
    $nonArm64Candidates = @()
    
    foreach ($candidate in $signtoolCandidates) {
        if (Test-IsArm64Binary -FilePath $candidate.FullName) {
            Write-Host "Found ARM64 signtool at $($candidate.FullName)"
            $prioritizedCandidates += $candidate
        } else {
            $nonArm64Candidates += $candidate
        }
    }
    
    # Combine ARM64 candidates first, then non-ARM64 candidates
    $signtoolCandidates = $prioritizedCandidates + $nonArm64Candidates
}

Write-Host "Found $($signtoolCandidates.Count) signtool candidates."
$files = Get-ChildItem .\* -Include ('*.msi') -File
Write-Host "Found $($files.Count) MSI installers to sign."

# Try each signtool until one works
$signedSuccessfully = $false
foreach ($signtoolCandidate in $signtoolCandidates) {
    $signtool = $signtoolCandidate.FullName
    Write-Host "Trying signtool at: $signtool"
    
    $allFilesSigned = $true
    
    foreach ($file in $files) {
        Write-Host "Signing $($file.FullName) with $signtool"
        & $signtool sign /tr http://timestamp.digicert.com /td sha256 /fd sha256 /d "Jexus Manager" /a $file.FullName 2>&1 | Write-Debug
        
        if ($LASTEXITCODE -ne 0) {
            Write-Host "Failed to sign $($file.FullName) with $signtool (exit code $LASTEXITCODE)"
            $allFilesSigned = $false
            break
        }
        
        # Verify immediately after signing
        & $signtool verify /pa /q $file.FullName 2>&1 | Write-Debug
        if ($LASTEXITCODE -ne 0) {
            Write-Host "Failed to verify $($file.FullName) with $signtool (exit code $LASTEXITCODE)"
            $allFilesSigned = $false
            break
        }
    }
    
    if ($allFilesSigned) {
        $signedSuccessfully = $true
        Write-Host "Successfully signed all MSI installers with $signtool"
        break
    }
}

if (-not $signedSuccessfully) {
    Write-Host "Failed to sign MSI installers with any available signtool. Exit."
    exit 1
}

Write-Host "Signing and verification finished successfully."
