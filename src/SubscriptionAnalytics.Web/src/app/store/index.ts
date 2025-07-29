import { ActionReducerMap } from "@ngrx/store";
import { AuthenticationState, authenticationReducer } from "./Authentication/authentication.reducer";
import { CandidateReducer, CandidateState } from "./Candidate/candidate.reducer";
import { CartReducer, CartState } from "./Cart/cart.reducer";
import { ChatReducer, ChatState } from "./Chat/chat.reducer";
import { OrderdataState, OrdersReducer } from "./Crypto/crypto.reducer";
import { CustomerReducer, CustomerState } from "./customer/customer.reducer";
import { MailReducer, MailState } from "./Email/email.reducer";
import { FilemanageReducer, FilemanagerState } from "./filemanager/filemanager.reducer";
import { InvoiceDataReducer, InvoiceDataState } from "./Invoices/invoices.reducer";
import { JoblistReducer, joblistState } from "./Job/job.reducer";
import { LayoutState, layoutReducer } from "./layouts/layouts.reducer";
import { EcoOrderState, OrderReducer } from "./orders/order.reducer";
import { projectReducer, projectState } from "./ProjectsData/project.reducer";
import { ProvidersState, providersReducer } from "./providers/providers.reducer";
import { tasklistReducer, tasklistState } from "./Tasks/tasks.reducer";
import { UserReducer, UserState } from "./UserGrid/user.reducer";
import { UserListReducer, UserlistState } from "./UserList/userlist.reducer";

export interface RootReducerState {
    layout: LayoutState;
    auth: AuthenticationState;
    Filelist: FilemanagerState;
    EcoOrderList: EcoOrderState;
    CartList: CartState;
    Projectlist: projectState;
    usergrid: UserState;
    userList: UserlistState;
    Joblist: joblistState;
    CandidateList: CandidateState;
    InvoiceList: InvoiceDataState;
    chatList: ChatState;
    Tasklist: tasklistState;
    Order: OrderdataState;
    Customer: CustomerState;
    Maillist: MailState;
    providers: ProvidersState;
}

export const rootReducer: ActionReducerMap<RootReducerState> = {
    layout: layoutReducer,
    auth: authenticationReducer,
    Filelist: FilemanageReducer,
    EcoOrderList: OrderReducer,
    CartList: CartReducer,
    Projectlist: projectReducer,
    usergrid: UserReducer,
    userList: UserListReducer,
    Joblist: JoblistReducer,
    CandidateList: CandidateReducer,
    InvoiceList: InvoiceDataReducer,
    chatList: ChatReducer,
    Tasklist: tasklistReducer,
    Order: OrdersReducer,
    Customer: CustomerReducer,
    Maillist: MailReducer,
    providers: providersReducer
}
