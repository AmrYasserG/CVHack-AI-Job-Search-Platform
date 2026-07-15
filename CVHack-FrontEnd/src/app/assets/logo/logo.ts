import { Component , Input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-logo',
  imports: [RouterLink],
  templateUrl: './logo.html',
  styleUrl: './logo.css',
})
export class Logo {
  @Input() href: string = '/jobs';
  @Input() cvColor: string = 'var(--white)';
}
