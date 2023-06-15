import { Component, Input } from '@angular/core';
import jwtDecode from 'jwt-decode';
import { Book } from 'src/app/models/book';
import { Jwt } from 'src/environments/const';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'app-book-card',
    templateUrl: './book-card.component.html',
    styleUrls: ['./book-card.component.scss']
})
export class BookCardComponent {
    @Input() book?: Book;
    public environment = environment;
    constructor() { }

    isAuthor(authorId?: string): boolean {
        if (authorId) {
            const token = localStorage.getItem('authToken');

            if (token) {
                const decodeToken: any = jwtDecode(token);

                if (decodeToken[Jwt.nameIdentifier] === authorId) {
                    return true;
                }
            }
        }

        return false;
    }
}
