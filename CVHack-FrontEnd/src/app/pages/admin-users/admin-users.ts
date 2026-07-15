import { Component, OnInit, inject, signal } from '@angular/core';
import { AdminSidebar } from '../../components/admin-sidebar/admin-sidebar';
import { AdminUsersHeader } from '../../components/admin-users-header/admin-users-header';
import { AdminUserDetailPanel } from '../../components/admin-user-detail-panel/admin-user-detail-panel';
import { AdminUsersList } from '../../components/admin-users-list/admin-users-list';
import { User } from '../../models/user.model';
import { AdminUsersService } from '../../services/admin-users.service';

@Component({
  selector: 'app-admin-users',
  imports: [AdminSidebar, AdminUsersHeader, AdminUsersList, AdminUserDetailPanel],
  templateUrl: './admin-users.html',
  styleUrl: './admin-users.css',
})
export class AdminUsers implements OnInit {
  private adminUsersService = inject(AdminUsersService);

  private allUsers: User[] = [];
  filteredUsers = signal<User[]>([]);
  selectedUser = signal<User | null>(null);
  isLoading = signal(true);
  errorMessage = signal('');

  ngOnInit() {
    this.adminUsersService.getUsers().subscribe({
      next: (users) => {
        this.allUsers = users;
        this.filteredUsers.set(users);
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('Failed to load users.');
        this.isLoading.set(false);
      },
    });
  }

  onSearch(term: string) {
    const q = term.toLowerCase();
    this.filteredUsers.set(
      this.allUsers.filter((u) =>
        `${u.firstName} ${u.lastName}`.toLowerCase().includes(q) ||
        u.email.toLowerCase().includes(q)
      )
    );
  }

  onUserSelected(user: User) {
    this.selectedUser.set(user);
  }
  onUserUpdated(updated: User) {
    this.allUsers = this.allUsers.map((u) => (u.id === updated.id ? updated : u));
    this.filteredUsers.set(
      this.filteredUsers().map((u) => (u.id === updated.id ? updated : u))
    );
    this.selectedUser.set(updated);
  }
}