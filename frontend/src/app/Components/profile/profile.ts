import { Component, signal, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService, AddressRequest } from '../../Services/user.service';
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
    isEditing?: boolean;
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

    // Address form state
    showAddressForm = signal(false);
    isEditingAddress = signal(false);
    editingAddressId = signal<number | null>(null);
    addressFormLoading = signal(false);

    // Address form fields
    addressType = 'Home';
    fullAddress = '';
    city = '';
    state = '';

    ngOnInit() {
        if (this.authService.isLoggedIn()) {
            this.loadUserProfile();
        } else {
            this.errorMessage.set('Please login to view your profile.');
            this.isLoading.set(false);
        }
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

                    this.fullNameBackup = this.fullName;
                    this.emailBackup = this.email;
                    this.phoneBackup = this.phone;

                    if (userData.addresses && Array.isArray(userData.addresses)) {
                        const addressList = userData.addresses.map((addr: any) => ({
                            id: addr.id,
                            type: addr.addressType || addr.type || 'Other',
                            address: addr.fullAddress || addr.address || '',
                            city: addr.city || '',
                            state: addr.state || '',
                            isEditing: false
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
            this.fullName = this.fullNameBackup;
            this.email = this.emailBackup;
            this.phone = this.phoneBackup;
            this.isEditing.set(false);
        } else {
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
                this.fullNameBackup = this.fullName;
                this.emailBackup = this.email;
                this.phoneBackup = this.phone;
                this.isEditing.set(false);
                this.successMessage.set('Profile updated successfully!');
                setTimeout(() => this.successMessage.set(''), 3000);
            },
            error: (error) => {
                console.error('Error updating profile:', error);
                this.errorMessage.set('Failed to update profile. Please try again.');
            }
        });
    }

    // Address Management Methods
    openAddAddressForm() {
        this.resetAddressForm();
        this.isEditingAddress.set(false);
        this.editingAddressId.set(null);
        this.showAddressForm.set(true);
    }

    openEditAddressForm(address: AddressInfo) {
        this.addressType = address.type || 'Other';
        this.fullAddress = address.address || '';
        this.city = address.city || '';
        this.state = address.state || '';
        this.isEditingAddress.set(true);
        this.editingAddressId.set(address.id || null);
        this.showAddressForm.set(true);
    }

    closeAddressForm() {
        this.showAddressForm.set(false);
        this.resetAddressForm();
    }

    resetAddressForm() {
        this.addressType = 'Home';
        this.fullAddress = '';
        this.city = '';
        this.state = '';
        this.isEditingAddress.set(false);
        this.editingAddressId.set(null);
    }

    saveAddress() {
        if (!this.fullAddress.trim() || !this.city.trim() || !this.state.trim()) {
            this.errorMessage.set('Please fill in all address fields.');
            setTimeout(() => this.errorMessage.set(''), 3000);
            return;
        }

        this.addressFormLoading.set(true);
        this.errorMessage.set('');

        const addressData: AddressRequest = {
            addressType: this.addressType,
            fullAddress: this.fullAddress.trim(),
            city: this.city.trim(),
            state: this.state.trim()
        };

        if (this.isEditingAddress() && this.editingAddressId()) {
            // Update existing address
            this.userService.updateAddress(this.editingAddressId()!, addressData).subscribe({
                next: (response) => {
                    this.successMessage.set('Address updated successfully!');
                    this.closeAddressForm();
                    this.loadUserProfile();
                    this.addressFormLoading.set(false);
                    setTimeout(() => this.successMessage.set(''), 3000);
                },
                error: (error) => {
                    console.error('Error updating address:', error);
                    this.errorMessage.set('Failed to update address. Please try again.');
                    this.addressFormLoading.set(false);
                }
            });
        } else {
            // Add new address
            this.userService.addAddress(addressData).subscribe({
                next: (response) => {
                    this.successMessage.set('Address added successfully!');
                    this.closeAddressForm();
                    this.loadUserProfile();
                    this.addressFormLoading.set(false);
                    setTimeout(() => this.successMessage.set(''), 3000);
                },
                error: (error) => {
                    console.error('Error adding address:', error);
                    this.errorMessage.set('Failed to add address. Please try again.');
                    this.addressFormLoading.set(false);
                }
            });
        }
    }

    deleteAddress(addressId: number) {
        if (!confirm('Are you sure you want to delete this address?')) {
            return;
        }

        this.userService.deleteAddress(addressId).subscribe({
            next: () => {
                this.successMessage.set('Address deleted successfully!');
                this.loadUserProfile();
                setTimeout(() => this.successMessage.set(''), 3000);
            },
            error: (error) => {
                console.error('Error deleting address:', error);
                this.errorMessage.set('Failed to delete address. Please try again.');
            }
        });
    }

    getAddressTypeDisplay(type: string): string {
        if (!type) return 'Other';
        const normalized = type.toUpperCase();
        if (normalized.includes('HOME')) return 'HOME';
        if (normalized.includes('WORK')) return 'WORK';
        return 'OTHER';
    }
}
