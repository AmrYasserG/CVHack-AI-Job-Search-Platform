import { Component,Output,EventEmitter } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MagnifyingGlassIcon } from "../../assets/magnifying-glass-icon/magnifying-glass-icon";

@Component({
  selector: 'app-admin-users-header',
  imports: [FormsModule, MagnifyingGlassIcon],
  templateUrl: './admin-users-header.html',
  styleUrl: './admin-users-header.css',
})
export class AdminUsersHeader {
  searchTerm = '';

  @Output() searchChange = new EventEmitter<string>();

  onSearch() {
    this.searchChange.emit(this.searchTerm);
  }
}
