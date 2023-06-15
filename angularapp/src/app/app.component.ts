import { Component } from '@angular/core';
import { Router } from '@angular/router';
import jwtDecode from 'jwt-decode';
import { Jwt } from 'src/environments/const';
import { AuthService } from './services/auth.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent {
    constructor(private router: Router, private authService: AuthService) { }

    get userName(): string | undefined {
        const token = localStorage.getItem('authToken');

        if (token) {
            const decodeToken: any = jwtDecode(token);
            return decodeToken[Jwt.name];
        }

        return undefined;
    }

    get isAuthorized(): boolean {
        const token = localStorage.getItem('authToken');

        if (token) {
            return !!jwtDecode(token);
        }

        return false;
    }

    public logout() {
        localStorage.removeItem('authToken');
        this.router.navigate(['']);
    }
}
