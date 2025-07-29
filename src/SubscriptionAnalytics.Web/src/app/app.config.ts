import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';




// Other module imports
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { provideEffects } from '@ngrx/effects';
import { provideStore } from '@ngrx/store';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { BsDropdownConfig } from 'ngx-bootstrap/dropdown';
import { provideToastr } from 'ngx-toastr';
import { environment } from '../environments/environment';
import { ApiModule } from './api-client/api.module';
import { Configuration } from './api-client/configuration';
import { routes } from './app.routes';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { tenantInterceptor } from './core/interceptors/tenant.interceptor';
import { tokenRefreshInterceptor } from './core/interceptors/token-refresh.interceptor';
import { rootReducer } from './store';
import { AuthenticationEffects } from './store/Authentication/authentication.effects';
import { CandidateEffects } from './store/Candidate/candidate.effects';
import { CartEffects } from './store/Cart/cart.effects';
import { ChatEffects } from './store/Chat/chat.effect';
import { OrdersEffects } from './store/Crypto/crypto.effects';
import { CustomerEffects } from './store/customer/customer.effects';
import { MailEffects } from './store/Email/email.effects';
import { FilemanagerEffects } from './store/filemanager/filemanager.effects';
import { InvoiceDataEffects } from './store/Invoices/invoice.effects';
import { JoblistEffects } from './store/Job/job.effects';
import { OrderEffects } from './store/orders/order.effects';
import { ProjectEffects } from './store/ProjectsData/project.effects';
import { ProvidersEffects } from './store/providers/providers.effects';
import { tasklistEffects } from './store/Tasks/tasks.effect';
import { usersEffects } from './store/UserGrid/user.effects';
import { userslistEffects } from './store/UserList/userlist.effect';

export function createTranslateLoader(http: HttpClient): any {
  return new TranslateHttpLoader(http, 'assets/i18n/', '.json');
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes),
    provideStore(rootReducer),
    provideEffects(
      [
        FilemanagerEffects,
        OrderEffects,
        AuthenticationEffects,
        CartEffects,
        ProjectEffects,
        usersEffects,
        userslistEffects,
        JoblistEffects,
        CandidateEffects,
        InvoiceDataEffects,
        ChatEffects,
        tasklistEffects,
        OrdersEffects,
        CustomerEffects,
        MailEffects,
        ProvidersEffects
      ]
    ),
    provideHttpClient(withInterceptors([authInterceptor, tenantInterceptor, tokenRefreshInterceptor])),
    importProvidersFrom(
      TranslateModule.forRoot({
        loader: {
          provide: TranslateLoader,
          useFactory: createTranslateLoader,
          deps: [HttpClient]
        }
      }),
    ),
    provideAnimations(),
    provideToastr(),
    { provide: BsDropdownConfig, useValue: { isAnimated: true, autoClose: true } },
    importProvidersFrom(
      ApiModule.forRoot(() => new Configuration({
        basePath: environment.apiBaseUrl
      }))
    )
  ]
};
