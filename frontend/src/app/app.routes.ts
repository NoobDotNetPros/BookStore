import { Routes } from '@angular/router';
import { ForgotPasswordComponent } from './forgot-password/forgot-password';
import { FormsModule } from '@angular/forms';

export const routes: Routes = [
    {
        path: 'forgot-password',
        component: ForgotPasswordComponent
    }
];
