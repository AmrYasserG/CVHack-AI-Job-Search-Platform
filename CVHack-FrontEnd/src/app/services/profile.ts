import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface ProfileData {
  id?: number;
  userId?: string;
  headline?: string;
  jobTitle?: string;
  city?: string;
  country?: string;
  phoneNumber?: string;
  summary?: string;
  gitHubUrl?: string;
  linkedInUrl?: string;
  portfolioUrl?: string;
  certifications?: any[];
  educations?: any[];
  experiences?: any[];
}

@Injectable({ providedIn: 'root' })
export class ProfileService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getProfile(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/profile`);
  }

  updateProfile(data: Partial<ProfileData>): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/profile`, data);
  }
}