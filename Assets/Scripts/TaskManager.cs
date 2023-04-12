#region Copyright Notice
// ******************************************************************************************************************
// 
// UnusualCommunication.TaskManager.cs © Shadow Wolf Development (SilentWolf6662 & Bambinidk) - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// 
// This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/
// 
// Created & Copyrighted @ 2023-04-12
// 
// ******************************************************************************************************************
#endregion
namespace UnusualCommunication
{
	/// This is a convenient coroutine API for Unity.
	///
	/// Example usage:
	///   IEnumerator MyAwesomeTask()
	///   {
	///       while(true) {
	///           // ...
	///           yield return null;
	////      }
	///   }
	///
	///   IEnumerator TaskKiller(float delay, Task t)
	///   {
	///       yield return new WaitForSeconds(delay);
	///       t.Stop();
	///   }
	///
	///   // From anywhere
	///   Task my_task = new Task(MyAwesomeTask());
	///   new Task(TaskKiller(5, my_task));
	///
	/// The code above will schedule MyAwesomeTask() and keep it running
	/// concurrently until either it terminates on its own, or 5 seconds elapses
	/// and triggers the TaskKiller Task that was created.
	///
	/// Note that to facilitate this API's behavior, a "TaskManager" GameObject is
	/// created lazily on first use of the Task API and placed in the scene root
	/// with the internal TaskManager component attached. All coroutine dispatch
	/// for Tasks is done through this component.

	using UnityEngine;
	using System.Collections;

	/// A Task object represents a coroutine.  Tasks can be started, paused, and stopped.
	/// It is an error to attempt to start a task that has been stopped or which has
	/// naturally terminated.
	public class Task
	{
		/// Returns true if and only if the coroutine is running.  Paused tasks
		/// are considered to be running.
		public bool Running => task.Running;

		/// Returns true if and only if the coroutine is currently paused.
		public bool Paused => task.Paused;

		/// Delegate for termination subscribers.  manual is true if and only if
		/// the coroutine was stopped with an explicit call to Stop().
		public delegate void FinishedHandler(bool manual);

		/// Termination event.  Triggered when the coroutine completes execution.
		public event FinishedHandler Finished;

		/// Creates a new Task object for the given coroutine.
		///
		/// If autoStart is true (default) the task is automatically started
		/// upon construction.
		public Task(IEnumerator c, bool autoStart = true)
		{
			task = TaskManager.CreateTask(c);
			task.Finished += TaskFinished;
			if (autoStart)
				Start();
		}

		/// Begins execution of the coroutine
		public void Start() => task.Start();

		/// Discontinues execution of the coroutine at its next yield.
		public void Stop() => task.Stop();

		public void Pause() => task.Pause();

		public void Unpause() => task.Unpause();

		private void TaskFinished(bool manual)
		{
			FinishedHandler handler = Finished;
			handler?.Invoke(manual);
		}

		private TaskManager.TaskState task;
	}

	internal class TaskManager : MonoBehaviour
	{
		public class TaskState
		{
			public bool Running => running;

			public bool Paused => paused;

			public delegate void FinishedHandler(bool manual);
			public event FinishedHandler Finished;

			private IEnumerator coroutine;
			private bool running;
			private bool paused;
			private bool stopped;

			public TaskState(IEnumerator c) => coroutine = c;

			public void Pause()
			{
				paused = true;
			}

			public void Unpause()
			{
				paused = false;
			}

			public void Start()
			{
				running = true;
				singleton.StartCoroutine(CallWrapper());
			}

			public void Stop()
			{
				stopped = true;
				running = false;
			}

			private IEnumerator CallWrapper()
			{
				yield return null;
				IEnumerator e = coroutine;
				while (running)
				{
					if (paused) yield return null;
					else if (e != null && e.MoveNext()) yield return e.Current;
					else running = false;
				}

				FinishedHandler handler = Finished;
				handler?.Invoke(stopped);
			}
		}

		private static TaskManager singleton;

		public static TaskState CreateTask(IEnumerator coroutine)
		{
			if (singleton == null)
			{
				GameObject go = new GameObject("TaskManager");
				singleton = go.AddComponent<TaskManager>();
			}
			return new TaskState(coroutine);
		}
	}
}
