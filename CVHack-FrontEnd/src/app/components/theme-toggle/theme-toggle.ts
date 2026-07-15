import { Component, inject } from '@angular/core';
import { MoonIcon } from '../../assets/moon-icon/moon-icon';
import { SunIcon } from '../../assets/sun-icon/sun-icon';
import { ThemeService } from '../../services/theme.service';

@Component({
  selector: 'app-theme-toggle',
  imports: [MoonIcon, SunIcon],
  templateUrl: './theme-toggle.html',
  styleUrls: ['./theme-toggle.css'],
})
export class ThemeToggle {
  private theme = inject(ThemeService);
  isDark = this.theme.isDark;   

  toggleTheme() {
    this.theme.toggle();
  }
}
