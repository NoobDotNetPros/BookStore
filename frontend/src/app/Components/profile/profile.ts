import { Component, signal, inject, OnInit, WritableSignal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../Services/user.service';
import { AuthService } from '../../Services/auth.service';

interface UserProfile {
    id?: number;
    fullName: string;
    email: string;
    phone: string;
}

interface AddressInfo {
    id?: number;
    type: string;
    address: string;
    city: string;
    state: string;
}

@Component({
    selector: 'app-profile',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './profile.html',
    styleUrl: './profile.scss',
})
export class Profile implements OnInit {
    private userService = inject(UserService);
    private authService = inject(AuthService);

    // User data
    fullName = '';
    email = '';
    phone = '';

    // Backup for cancel functionality
    private fullNameBackup = '';
    private emailBackup = '';
    private phoneBackup = '';

    addresses = signal<AddressInfo[]>([]);
    isEditing = signal(false);
    isLoading = signal(true);
    errorMessage = signal('');
    successMessage = signal('');

    ngOnInit() {
        this.loadUserProfile();
    }

    loadUserProfile() {
        this.isLoading.set(true);
        this.errorMessage.set('');
        this.successMessage.set('');

        this.userService.getUserProfile().subscribe({
            next: (response) => {
                if (response.data) {
                    const userData = response.data;
                    this.fullName = userData.fullName || '';
                    this.email = userData.email || '';
                    this.phone = userData.phone || userData.mobileNumber || '';

                    // Store backup
                    this.fullNameBackup = this.fullName;
                    this.emailBackup = this.email;
                    this.phoneBackup = this.phone;

                    // Load addresses if available
                    if (userData.addresses && Array.isArray(userData.addresses)) {
                        const addressList = userData.addresses.map((addr: any) => ({
                            id: addr.id,
                            type: addr.addressType || addr.type || '',
                            address: addr.fullAddress || addr.address || '',
                            city: addr.city || '',
                            state: addr.state || ''
                        }));
                        this.addresses.set(addressList);
                    } else {
                        this.addresses.set([]);
                    }
                }
                this.isLoading.set(false);
            },
            error: (error) => {
                console.error('Error loading user profile:', error);
                this.errorMessage.set('Failed to load profile. Please try again.');
                this.isLoading.set(false);
            }
        });
    }

    toggleEdit() {
        if (this.isEditing()) {
            // Cancel editing - restore backup
            this.fullName = this.fullNameBackup;
            this.email = this.emailBackup;
            this.phone = this.phoneBackup;
            this.isEditing.set(false);
        } else {
            // Enter edit mode - create backup
            this.fullNameBackup = this.fullName;
            this.emailBackup = this.email;
            this.phoneBackup = this.phone;
            this.isEditing.set(true);
        }
    }

    saveProfile() {
        this.errorMessage.set('');
        this.successMessage.set('');

        const updateData = {
            fullName: this.fullName,
            email: this.email,
            phone: this.phone
        };

        this.userService.updateUserProfile(updateData).subscribe({
            next: (response) => {
                // Update backups on success
                this.fullNameBackup = this.fullName;
                this.emailBackup = this.email;
                this.phoneBackup = this.phone;
                this.isEditing.set(false);
                this.successMessage.set('Profile updated successfully!');

                // Clear success message after 3 seconds
                setTimeout(() => this.successMessage.set(''), 3000);
            },
            error: (error) => {
                console.error('Error updating profile:', error);
                this.errorMessage.set('Failed to update profile. Please try again.');
            }
        });
    }

    addNewAddress() {
        console.log('Add new address clicked');
        // TODO: Implement add address functionality
    }
}
