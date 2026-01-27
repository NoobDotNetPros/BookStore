import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-profile',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './profile.html',
    styleUrl: './profile.scss',
})
export class Profile {
    user = signal({
        fullName: 'Poonam Yadav',
        email: 'Poonam.Yadav@bridgelabz.com',
        password: 'password123',
        mobile: '81678954778'
    });

    addresses = signal([
        {
            id: 1,
            type: '1.WORK',
            address: 'BridgeLabz Solutions LLP, No. 42, 14th Main, 15th Cross, Sector 4, Opp to BDA complex, near Kumarakom restaurant, HSR Layout, Bangalore',
            city: 'Bengaluru',
            state: 'Karnataka'
        }
    ]);

    isEditing = signal(false);

    toggleEdit() {
        this.isEditing.update(val => !val);
    }

    addNewAddress() {
        console.log('Add new address clicked');
    }
}
