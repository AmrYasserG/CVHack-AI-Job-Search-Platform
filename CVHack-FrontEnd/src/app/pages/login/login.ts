import { Component } from '@angular/core';
import { AuthCard } from '../../components/auth-card/auth-card';
import { LoginForm } from '../../components/login-form/login-form';
import { FeaturePills } from '../../components/feature-pills/feature-pills';
import { ThemeToggle } from '../../components/theme-toggle/theme-toggle';

@Component({
  selector: 'app-login',
  imports: [AuthCard, LoginForm, FeaturePills],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {}
