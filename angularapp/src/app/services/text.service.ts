import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Text } from '../models/text';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root'
})
export class TextService {
    private url = "Text";

    constructor(private httpClient: HttpClient) { }

    public get(id: string): Observable<Text> {
        return this.httpClient.get<Text>(`${environment.apiUrl}/${this.url}/${id}`);
    }
}
