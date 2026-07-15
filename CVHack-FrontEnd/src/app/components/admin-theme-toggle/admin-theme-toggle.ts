import { Component, inject } from '@angular/core';
import { ThemeService } from '../../services/theme.service';
import { MoonIcon } from "../../assets/moon-icon/moon-icon";
import { SunIcon } from "../../assets/sun-icon/sun-icon";

@Component({
  selector: 'app-admin-theme-toggle',
  imports: [MoonIcon, SunIcon],
  templateUrl: './admin-theme-toggle.html',
  styleUrl: './admin-theme-toggle.css',
})
export class AdminThemeToggle {
  private theme = inject(ThemeService);
  isDark = this.theme.isDark;

  toggleTheme() {
    this.theme.toggle();
  }
}
