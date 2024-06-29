import { Role } from "../enums/Role";

export interface User{
    userId: string;
    userName: string;
    password: string;
    email:string;
    role:Role;
}