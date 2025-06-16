import { Roles } from "./roles.enum";

export interface User{
    userId: number;
    userName: string;
    role: Roles;
    profileImgUrl: string | null;
}