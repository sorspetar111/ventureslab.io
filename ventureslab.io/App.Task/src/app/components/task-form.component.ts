import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskService } from '../../services/task.service';
import { TaskModel } from '../../models/task.model';

@Component({
  selector: 'app-task-form',
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.css']
})
export class TaskFormComponent implements OnInit {
  task: TaskModel = { id: 0, userId: 1, start: new Date(), end: new Date(), subject: '', description: '' };
  isEditMode = false;

  constructor(
    private taskService: TaskService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const taskId = this.route.snapshot.paramMap.get('id');
    if (taskId) {
      this.isEditMode = true;
      this.taskService.getTask(+taskId).subscribe(task => this.task = task);
    }
  }

  saveTask(): void {
    if (this.isEditMode) {
      this.taskService.updateTask(this.task.id, this.task).subscribe(() => this.router.navigate(['/task-list']));
    } else {
      this.taskService.createTask(this.task).subscribe(() => this.router.navigate(['/task-list']));
    }
  }
}
