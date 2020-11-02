import Vue from 'vue'
import { Button,Form,FormItem,Input,Container,Header,Aside,Main,Menu,Submenu,MenuItemGroup, MenuItem,RadioButton,RadioGroup,Breadcrumb,BreadcrumbItem,Card,Table,TableColumn,Row,Col,Pagination,Tooltip,Dialog,MessageBox,Switch} from 'element-ui'

//弹框提示
import{Message} from 'element-ui'
Vue.use(Button)
Vue.use(Form)
Vue.use(FormItem)
Vue.use(Input)
Vue.use(Container)
Vue.use(Header)
Vue.use(Aside)
Vue.use(Main)
Vue.use(Menu)
Vue.use(Submenu)
Vue.use(MenuItemGroup)
Vue.use(MenuItem)
Vue.use(RadioButton)
Vue.use(RadioGroup)
Vue.use(Breadcrumb)
Vue.use(BreadcrumbItem)
Vue.use(Card)
Vue.use(Table)
Vue.use(TableColumn)
Vue.use(Row)
Vue.use(Col)
Vue.use(Pagination)
Vue.use(Tooltip)
Vue.use(Dialog)
Vue.use(Switch)
//全局挂载
Vue.prototype.$message=Message
//Messagebox弹窗
Vue.prototype.$confirm=MessageBox.confirm