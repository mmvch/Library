import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Credentials } from '../models/credentials';
import { Token } from '../models/token';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
    private url = "Auth";
    constructor(private httpClient: HttpClient) { }

    public register(user: Credentials): Observable<Token> {
        return this.httpClient.post<Token>(`${environment.apiUrl}/${this.url}/register`, user);
    }

    public login(user: Credentials): Observable<Token> {
        return this.httpClient.post<Token>(`${environment.apiUrl}/${this.url}/login`, user);
    }
}
