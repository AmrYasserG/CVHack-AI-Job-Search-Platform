import { Component, Input, Output, EventEmitter, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Observable } from 'rxjs';
import { User } from '../../models/user.model';
import { AdminUsersService } from '../../services/admin-users.service';
import { FullNamePipe } from '../../pipes/full-name-pipe';
import { InitialsPipe } from '../../pipes/initials-pipe';
import { ConfirmDialog } from '../confirm-dialog/confirm-dialog';


@Component({
  selector: 'app-admin-user-detail-panel',
  imports: [DatePipe, FullNamePipe, InitialsPipe, ConfirmDialog],
  templateUrl: './admin-user-detail-panel.html',
  styleUrl: './admin-user-detail-panel.css',
})
export class AdminUserDetailPanel {
  @Input() user: User | null = null;
  @Output() userUpdated = new EventEmitter<User>();

  private adminUsersService = inject(AdminUsersService);

  loading = signal(false);
  errorMessage = signal('');

  // modal state
  confirmOpen = signal(false);
  confirmTitle = signal('');
  confirmMessage = signal('');
  confirmText = signal('Confirm');
  private pendingAction: (() => void) | null = null;

  get isAdmin(): boolean { return !!this.user?.roles.includes('Admin'); }
  get isSuspended(): boolean { return this.user?.status === 'Suspended'; }

  askMakeAdmin() {
    if (!this.user || this.isAdmin) return;
    this.openConfirm(
      'Make Admin',
      `Promote ${this.user.firstName} ${this.user.lastName} to admin? This can't be undone.`,
      'Make Admin',
      () => this.run(this.adminUsersService.promoteToAdmin(this.user!.id))
    );
  }

  askToggleStatus() {
    if (!this.user || this.isAdmin) return;
    const next: 'Active' | 'Suspended' = this.isSuspended ? 'Active' : 'Suspended';
    this.openConfirm(
      next === 'Suspended' ? 'Suspend User' : 'Activate User',
      `${next === 'Suspended' ? 'Suspend' : 'Activate'} ${this.user.firstName} ${this.user.lastName}?`,
      next === 'Suspended' ? 'Suspend' : 'Activate',
      () => this.run(this.adminUsersService.updateStatus(this.user!.id, next))
    );
  }

  askTogglePlan() {
    if (!this.user || this.isAdmin) return;
    const next: 'Free' | 'Pro' = this.user.plan === 'Pro' ? 'Free' : 'Pro';
    this.openConfirm(
      'Change Plan',
      `Change ${this.user.firstName}'s plan to ${next}?`,
      'Change Plan',
      () => this.run(this.adminUsersService.updatePlan(this.user!.id, next))
    );
  }

  onConfirm() {
    this.confirmOpen.set(false);
    const action = this.pendingAction;
    this.pendingAction = null;
    action?.();
  }

  onCancel() {
    this.confirmOpen.set(false);
    this.pendingAction = null;
  }

  private openConfirm(title: string, message: string, confirmText: string, action: () => void) {
    this.confirmTitle.set(title);
    this.confirmMessage.set(message);
    this.confirmText.set(confirmText);
    this.pendingAction = action;
    this.confirmOpen.set(true);
  }

  private run(obs: Observable<User>) {
    this.loading.set(true);
    this.errorMessage.set('');
    obs.subscribe({
      next: (updated) => { this.loading.set(false); this.userUpdated.emit(updated); },
      error: (err) => { this.loading.set(false); this.errorMessage.set(err.error?.message || 'Action failed.'); },
    });
  }
}