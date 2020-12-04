import axios from './axios.config'

//菜单列表
const getMenu=()=>axios.get("/Page/GetMenu").then(res=>{
    return res.data
});

const page={
    getMenu
}
export default page