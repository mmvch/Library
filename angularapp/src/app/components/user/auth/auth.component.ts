import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Credentials } from 'src/app/models/credentials';
import { Token } from 'src/app/models/token';
import { AuthService } from 'src/app/services/auth.service';

@Component({
    selector: 'app-auth',
    templateUrl: './auth.component.html',
    styleUrls: ['./auth.component.scss']
})
export class AuthComponent implements OnInit {
    @Input() user = new Credentials();
    isLogin = false;

    constructor(private route: ActivatedRoute, private router: Router, private toastr: ToastrService, private authService: AuthService) { }

    ngOnInit(): void {
        this.isLogin = this.route.snapshot.routeConfig?.path?.endsWith('login') ?? false;
    }

    public register(user: Credentials) {
        this.authService.register(user).subscribe({
            next: (token: Token) => {
                localStorage.setItem('authToken', token.data!);
                this.return();
            },
            error: (error: any) => {
                this.toastr.error(error.error.message);
            },
            complete: () => {
                this.toastr.success("Login successful.");
            }
        });
    }

    public login(user: Credentials) {
        this.authService.login(user).subscribe({
            next: (token: Token) => {
                localStorage.setItem('authToken', token.data!);
                this.return();
            },
            error: (error: any) => {
                this.toastr.error(error.error.message);
            },
            complete: () => {
                this.toastr.success("Login successful.");
            }
        });
    }

    return() {
        this.router.navigate(['']);
    }
}
