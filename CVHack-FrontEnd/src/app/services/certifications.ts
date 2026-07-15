import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface CertificationItem {
  id?: string;
  name: string;
  provider?: string | null;
  credentialUrl?: string | null;
  certifiedAt?: string | null;
}

@Injectable({
  providedIn: 'root'
})
export class CertificationsService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({ Authorization: `Bearer ${token}` });
  }

  getCertifications(): Observable<any> {
  return this.http.get<any>(`${this.apiUrl}/profile/certifications`);
}

addCertification(data: CertificationItem): Observable<any> {
  return this.http.post<any>(`${this.apiUrl}/profile/certifications`, data);
}

updateCertification(id: string, data: CertificationItem): Observable<any> {
  return this.http.put<any>(`${this.apiUrl}/profile/certifications/${id}`, data);
}

deleteCertification(id: string): Observable<any> {
  return this.http.delete<any>(`${this.apiUrl}/profile/certifications/${id}`);
}
}