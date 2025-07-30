import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { TenantGuard } from './core/guards/tenant.guard';
import { CyptolandingComponent } from './cyptolanding/cyptolanding.component';
import { Page404Component } from './extrapages/page404/page404.component';
import { LayoutComponent } from './layouts/layout.component';

export const routes: Routes = [
    {
        path: "",
        redirectTo: "/app",
        pathMatch: "full"
    },
    {
        path: "tenant",
        component: LayoutComponent,
        loadChildren: () =>
            import("./pages/tenant/tenant.module").then((m) => m.TenantModule),
        canActivate: [AuthGuard],
    },
    {
        path: "app",
        component: LayoutComponent,
        loadChildren: () =>
            import("./pages/pages.module").then((m) => m.PagesModule),
        canActivate: [AuthGuard, TenantGuard],
    },
    {
        path: "providers",
        component: LayoutComponent,
        loadChildren: () =>
            import("./pages/providers/providers.module").then((m) => m.ProvidersModule),
        canActivate: [AuthGuard, TenantGuard],
    },
    {
        path: "auth",
        loadChildren: () =>
            import("./account/account.module").then((m) => m.AccountModule),
    },
    {
        path: "pages",
        loadChildren: () =>
            import("./extrapages/extrapages.module").then((m) => m.ExtrapagesModule),
        canActivate: [AuthGuard],
    },
    { path: "crypto-ico-landing", component: CyptolandingComponent },
    { path: "**", component: Page404Component },
];
