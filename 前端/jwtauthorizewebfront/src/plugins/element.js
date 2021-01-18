import Vue from 'vue'
import { Button,Form,FormItem,Menu,Notification,MenuItem,Submenu,Breadcrumb,BreadcrumbItem,Tooltip,DropdownMenu,DropdownItem,Avatar,Dropdown,Tabs,Dialog,Upload,Input,Tag,Message,Select,Option,Table,TableColumn} from 'element-ui'

Vue.use(Button)
Vue.use(Form)
Vue.use(FormItem)
Vue.use(Menu)
Vue.use(MenuItem)
Vue.use(Submenu)
Vue.use(Breadcrumb)
Vue.use(BreadcrumbItem)
Vue.use(Tooltip)
Vue.use(DropdownMenu)
Vue.use(DropdownItem)
Vue.use(Avatar)
Vue.use(Dropdown)
Vue.use(Tabs)
Vue.use(Dialog)
Vue.use(Upload)
Vue.use(Input)
Vue.use(Tag)
Vue.use(Select)
Vue.use(Option)
Vue.use(Table)
Vue.use(TableColumn)
//右边弹窗
Vue.prototype.$notify = Notification;

Vue.prototype.$message = Message;