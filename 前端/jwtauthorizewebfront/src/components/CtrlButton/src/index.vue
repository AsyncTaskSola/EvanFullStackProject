<template>
  <div class="buttons">
    <transition
      name="slide-bottom"
      v-for="(btn, i) in Button"
      :key="i"
      tag="div"
    >
      <el-tooltip
        v-if="Showbutton"
        effect="dark"
        :content="btn.name"
        placement="right"
      >
        <el-button
          :type="btn.type"
          size="mini"
          :icon="btn.icon"
          circle
          :style="{ transitionDelay: `0.${i}s` }"
          @click="handlerClick(btn.methods)"
        ></el-button>
      </el-tooltip>
    </transition>
  </div>
</template>

<script>
let self;
export default {
  name: "CtrlButton",
  props: ["Showbutton", "Button"],
  created() {
    self = this;
  },
  data() {
    return {};
  },
  methods: {
    handlerClick(n) {
      self.$emit("Oclick", n);
    },
  },
};
</script>

<style lang="scss" scpoed>
.buttons {
  position: relative;
  flex: 0 0 30px;
  margin-left: 10px;
  display: flex;
  flex-direction: column;
  align-items: center;
  z-index: 2;
  /deep/ .el-button {
    margin: 0;
    margin-bottom: 10px;
  }
}
.slide-bottom-enter-active {
  transition: all 0.3s ease;
}
.slide-bottom-leave-active {
  will-change: transform;
  transition: all 0.8s cubic-bezier(1, 0.5, 0.8, 1);
  z-index: 1;
}
.slide-bottom-enter, .slide-bottom-leave-to
/* .slide-fade-leave-active for below version 2.1.8 */ {
  opacity: 0;
  transform: translate3d(0, -100%, 0);
}
</style>