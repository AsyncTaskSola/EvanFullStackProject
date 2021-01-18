import * as types from './types' 

const mutations={
    [types.GETROLES](state,result){
        state.roles=result.data;
    }
}
export default mutations