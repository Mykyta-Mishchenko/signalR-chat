import { inject, Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, GuardResult, Router, RouterStateSnapshot } from "@angular/router";
import { AuthService } from "../services/auth/auth.service";

@Injectable({ providedIn: 'root' })
export class CanActivateAuthorizedPages implements CanActivate{
    private router = inject(Router);
    private authService = inject(AuthService);
    
    async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<GuardResult> {
        const accessToken = await this.authService.checkTokenValidity();
        if (this.authService.isLoggedIn()) {
            this.authService.updateUserByToken(accessToken);
            return true;
        }

        this.router.navigateByUrl('/auth/signIn');
        return false;
    }
}