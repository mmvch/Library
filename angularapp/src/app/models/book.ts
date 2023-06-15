import { User } from "./user";

export class Book {
    id?: string;
    name?: string;
    description?: string;
    creationDate?: string;
    image?: string;
    user?: User;
}
