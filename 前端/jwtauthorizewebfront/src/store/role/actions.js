import * as types from './types';
import api from '@/api'
const actions={
    async [types.GETROLES]({commit},{pageindex,pageSize}){
        const result=await api.role.rolesresult(pageindex,pageSize);
        commit(types.GETROLES,result);
    },
    
    //更新当前用户的角色（本应该是最高等级才会显示）
    async [types.UpDATEURINFO]({commit},entity)
    {
        console.log('Entity',entity);
        await api.role.ChangeRole(entity);
    }
}

export default actions

