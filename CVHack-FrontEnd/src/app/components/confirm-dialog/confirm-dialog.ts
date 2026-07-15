import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-confirm-dialog',
  imports: [],
  templateUrl: './confirm-dialog.html',
  styleUrl: './confirm-dialog.css',
})
export class ConfirmDialog {
  open = input(false);
  title = input('Are you sure?');
  message = input('');
  confirmText = input('Confirm');

  confirmed = output<void>();
  cancelled = output<void>();
}
