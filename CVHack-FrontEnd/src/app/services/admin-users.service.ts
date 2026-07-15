import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResult } from '../models/auth.model';
import { User } from '../models/user.model';


@Injectable({ providedIn: 'root' })
export class AdminUsersService {
    private http = inject(HttpClient);
    private apiUrl = environment.apiUrl;

    getUsers(): Observable<User[]> {
        return this.http
            .get<ApiResult<User[]>>(`${this.apiUrl}/admin/users`)
            .pipe(map((res) => res.data ?? []));
    }
    promoteToAdmin(id: string): Observable<User> {
        return this.http
            .post<ApiResult<User>>(`${this.apiUrl}/admin/users/${id}/promote`, {})
            .pipe(map((res) => res.data!));
    }

    updateStatus(id: string, status: 'Active' | 'Suspended'): Observable<User> {
        return this.http
            .put<ApiResult<User>>(`${this.apiUrl}/admin/users/${id}/status`, { status })
            .pipe(map((res) => res.data!));
    }

    updatePlan(id: string, plan: 'Free' | 'Pro'): Observable<User> {
        return this.http
            .put<ApiResult<User>>(`${this.apiUrl}/admin/users/${id}/plan`, { plan })
            .pipe(map((res) => res.data!));
    }

}