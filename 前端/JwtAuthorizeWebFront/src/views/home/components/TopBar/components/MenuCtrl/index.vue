<template>
  <div class="menuCtrl" @click="handerClick">
    <i class="icon" :class="fold ? 'el-icon-s-unfold' : 'el-icon-s-fold'"></i>
  </div>
</template>

<script>
export default {
name:'MenuCtrl',
data(){
    return {
        fold:false
    }
},
methods:{
    handerClick(){
        this.fold=!this.fold;
        this.$store.commit('global/menuCtrl',this.fold)
    }
}
}
</script>

<style lang="scss" scoped>
.menuCtrl {
  margin-right: 10px;
  cursor: pointer;
  .icon {
    font-size: 20px;
  }
}
</style>