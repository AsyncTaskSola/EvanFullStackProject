import * as types from './types'

const mutations={
    //错误与信息
    [types.ERROR_RES](state,{status,msg}){      
        state.errorRes=status;
        state.errorMsg=msg;
    },
    //获取菜单
    [types.SET_MENUS](state,menus)
    {
        state.menus=menus.data;
        let map={};
        getOneArr(map,menus.data);
        console.log('拆分菜单',map);       
        state.menusMap = map;
    },
    // 菜单的图标
    [types.MENU_CTRL](state,fold)
    {
        state.fold=fold;
    }
}

//拆分菜单
function getOneArr(map,arr){
    arr.forEach(x => {
        map[x.module]=x.name;      
        if(x.children.length)
        {
            console.log('子集',x.children);     
            getOneArr(map, x.children);
        }
    });
}
export default mutations;