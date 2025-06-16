import { Routes } from '@angular/router';
import { CanActivateSignPages } from './core/guards/sign-in.guard';
import { routes as authorizationRoutes } from './features/authorization/authorization.route';
import { CanActivateAuthorizedPages } from './core/guards/auth.guard';
import { ChatMarkupComponent } from './features/chat/chat-markup/chat-markup.component';

export const routes: Routes = [
    {
        path: 'auth',
        canActivate: [CanActivateSignPages],
        children: authorizationRoutes
    },
    {
        path: '',
        redirectTo: 'chat',
        pathMatch: 'full'
    },
    {
        path: 'chat',
        canActivate: [CanActivateAuthorizedPages],
        component: ChatMarkupComponent
    }
];