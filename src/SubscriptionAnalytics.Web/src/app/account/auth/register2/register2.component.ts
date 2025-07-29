import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';


import { CommonModule } from '@angular/common';
import { Store } from '@ngrx/store';
import { SlickCarouselModule } from 'ngx-slick-carousel';
import { Register } from 'src/app/store/Authentication/authentication.actions';
import { UserProfileService } from '../../../core/services/user.service';

@Component({
  selector: 'app-register2',
  templateUrl: './register2.component.html',
  styleUrls: ['./register2.component.scss'],
  standalone:true,
  imports:[CommonModule,FormsModule,ReactiveFormsModule,SlickCarouselModule ]
})
export class Register2Component implements OnInit {

  signupForm: UntypedFormGroup;
  submitted: any = false;
  error: any = '';
  successmsg: any = false;

  constructor(private formBuilder: UntypedFormBuilder, private route: ActivatedRoute, private router: Router,
    private userService: UserProfileService, public store: Store) { }
  // set the currenr year
  year: number = new Date().getFullYear();

  ngOnInit(): void {
    document.body.classList.add("auth-body-bg");

    this.signupForm = this.formBuilder.group({
      email: ['admin@themesbrand.com', [Validators.required, Validators.email]],
      password: ['Asdf@12345', [Validators.required, Validators.minLength(6)]],
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.signupForm.controls; }

  // swiper config
  slideConfig = {
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: false,
    dots: true
  };

  /**
   * On submit form
   */
  onSubmit() {
    this.submitted = true;

    const email = this.f['email'].value;
    const password = this.f['password'].value;

    //Dispatch Action
    this.store.dispatch(Register({ email: email, password: password }));
  }
}
