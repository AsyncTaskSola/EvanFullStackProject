//自定义组件

import Vue from 'vue';
import Cell from './components/Cell'
import CtrlButton from './components/CtrlButton'
const components=[Cell,CtrlButton];
components.forEach(component=>{
    Vue.use(component)
})