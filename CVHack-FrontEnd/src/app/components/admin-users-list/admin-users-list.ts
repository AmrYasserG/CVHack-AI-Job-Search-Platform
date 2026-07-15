import { Component, Input, Output, EventEmitter } from '@angular/core';
import { User } from '../../models/user.model';
import { FullNamePipe } from '../../pipes/full-name-pipe';
import { InitialsPipe } from '../../pipes/initials-pipe';

@Component({
  selector: 'app-admin-users-list',
  imports: [FullNamePipe,InitialsPipe],
  templateUrl: './admin-users-list.html',
  styleUrl: './admin-users-list.css',
})
export class AdminUsersList {
  @Input() users: User[] = [];
  @Input() selectedUserId: string | null = null;

  @Output() userSelected = new EventEmitter<User>();

  select(user: User) {
    this.userSelected.emit(user);
  }
}
