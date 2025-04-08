// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Web.Management.Client
{
    public sealed class TaskListCollection : CollectionBase
    {
        private readonly List<TaskList> _inner;

        public TaskListCollection()
        {
            _inner = new List<TaskList>();
        }

        public TaskListCollection(ICollection taskLists)
            : this()
        {
            foreach (TaskList item in taskLists)
            {
                Add(item);
            }
        }

        public int Add(TaskList taskList)
        {
            if (_inner.Contains(taskList))
            {
                return -1;
            }

            _inner.Add(taskList);
            return _inner.Count - 1;
        }

        public bool Contains(TaskList taskList)
        {
            return _inner.Contains(taskList);
        }

        public void CopyTo(TaskList[] taskLists, int index)
        {
            _inner.CopyTo(taskLists, index);
        }

        public int IndexOf(TaskList taskList)
        {
            return _inner.IndexOf(taskList);
        }

        public void Insert(int index, TaskList taskList)
        {
            _inner.Insert(0, taskList);
        }

        public void Remove(TaskList taskList)
        {
            _inner.Remove(taskList);
        }

        internal IEnumerable<KeyValuePair<TaskList, ICollection>> GetTaskListItems(IServiceProvider serviceProvider)
        {
            return _inner.Select(item => new KeyValuePair<TaskList, ICollection>(item, item.GetTaskItems()));
        }

        public TaskList this[int index]
        {
            get { return _inner[index]; }
            set { _inner[index] = value; }
        }
    }
}
