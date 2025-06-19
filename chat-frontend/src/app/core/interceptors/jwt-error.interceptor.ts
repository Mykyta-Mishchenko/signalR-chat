import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { AuthService } from "../services/auth/auth.service";
import { Router } from "@angular/router";
import { catchError, Observable, switchMap, throwError } from "rxjs";

@Injectable()
export class JwtErrorInterceptor implements HttpInterceptor{
    private authService = inject(AuthService);
    private router = inject(Router);

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(catchError(error => {
            if (error instanceof HttpErrorResponse && error.status == 401) {
                if (!req.url.includes('/signIn') && !req.url.includes('/auth/refresh')) {
                    return this.handle401Error(req, next);
                }
            }
            
            return throwError(() => error);
        })); 
    }

    private handle401Error(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return this.authService.refreshToken().pipe(
            switchMap(() => {
                return next.handle(this.addToken(req))
            }),
            catchError((error) => {
                console.error("Failed to refresh token:", error);
                this.authService.logout();
                this.router.navigate(['/auth/signIn']);
                return throwError(() => error);
            })
        )
    }

    private addToken(req: HttpRequest<any>): HttpRequest<any>{
        const accessToken = this.authService.ACCESS_TOKEN();
        if (accessToken) {
            return req.clone({
                setHeaders: {
                    Authorization: `Bearer ${accessToken}`
                }
            });
        }

        return req;
    }

}