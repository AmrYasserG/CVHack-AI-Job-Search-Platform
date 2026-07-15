import { Component, Input } from '@angular/core';



@Component({
  selector: 'app-profile-header',
  standalone: true,
  imports: [],
  templateUrl: './profile-header.html',
  styleUrl: './profile-header.css'
})
export class ProfileHeader {
  @Input() fullName: string = '';
  @Input() jobTitle: string = '';
  @Input() location: string = '';
  @Input() initials: string = '';

  // onDownloadCV() {
  //   console.log('Downloading CV...');
    
  // }
}