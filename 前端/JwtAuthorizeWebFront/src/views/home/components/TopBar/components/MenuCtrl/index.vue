<template>
  <div class="menuCtrl" @click="handerClick">
    <i class="icon" :class="fold"></i>
  </div>
</template>

<script>
export default {
name:'MenuCtrl',
data(){
    return {
        flod=false
    }
},
methods:{
    handerClick(){
        this.flod=!this.flod;
        this.$store.commit()
    }
}
}
</script>

<style>
</style>