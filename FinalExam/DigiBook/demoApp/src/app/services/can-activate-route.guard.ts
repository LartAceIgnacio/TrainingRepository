import { Injectable } from '@angular/core';
import { CanActivate,
         ActivatedRouteSnapshot,
         RouterStateSnapshot } from '@angular/router';
import { AuthService } from "./auth.service";
import { Router } from "@angular/router";


@Injectable()
export class CanActivateRouteGuard implements CanActivate {

  constructor(private auth: AuthService, private router:Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
      if(this.auth.isLoggedIn()){
        this.router.navigate(['dashboard']);
        console.log(this.auth.isLoggedIn());
        return true;
      }
      else{
        this.router.navigate(['login']);
        return false;
      }
      
  }
}