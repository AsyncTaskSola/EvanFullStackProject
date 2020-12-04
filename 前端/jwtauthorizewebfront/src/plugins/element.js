import Vue from 'vue'
import { Button,Form,FormItem,Menu,Notification,MenuItem,Submenu } from 'element-ui'

Vue.use(Button)
Vue.use(Form)
Vue.use(FormItem)
Vue.use(Menu)
Vue.use(MenuItem)
Vue.use(Submenu)

Vue.prototype.$notify = Notification;