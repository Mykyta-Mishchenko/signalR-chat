import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { AuthService } from "../services/auth/auth.service";
import { Observable } from "rxjs";

@Injectable()
export class JwtInterceptor implements HttpInterceptor{
    private authService = inject(AuthService);

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const isLoggedIn = this.authService.isLoggedIn;
        if (isLoggedIn()) {
            req = req.clone({
                setHeaders: {
                    Authorization: `Bearer ${this.authService.ACCESS_TOKEN()}`
                }
            });   
        }

        return next.handle(req);
    }
}