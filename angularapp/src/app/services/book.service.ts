import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Book } from '../models/book';
import { PartialData } from '../models/partial-data';

@Injectable({
    providedIn: 'root'
})
export class BookService {
    private url = "Book";

    constructor(private httpClient: HttpClient) { }

    public list(params: HttpParams): Observable<PartialData<Book>> {
        return this.httpClient.get<PartialData<Book>>(`${environment.apiUrl}/${this.url}`, { params });
    }

    public get(id: string): Observable<Book> {
        return this.httpClient.get<Book>(`${environment.apiUrl}/${this.url}/${id}`);
    }

    public create(formData: FormData): Observable<Book> {
        return this.httpClient.post<Book>(`${environment.apiUrl}/${this.url}`, formData);
    }

    public update(formData: FormData): Observable<Book> {
        return this.httpClient.put<Book>(`${environment.apiUrl}/${this.url}`, formData);
    }

    public delete(id: string): Observable<Book> {
        return this.httpClient.delete<Book>(`${environment.apiUrl}/${this.url}/${id}`);
    }
}
