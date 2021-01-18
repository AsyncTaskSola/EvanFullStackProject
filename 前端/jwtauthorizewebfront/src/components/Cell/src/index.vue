<template>
  <div class="cell">
    <p class="label">{{ this.label }}</p>
    <p class="value">{{ this.value }}</p>
  </div>
</template>

<script>
export default {
  name: "Cell",
  props: ["label", "value"],
  data() {
    return {};
  },
};
</script>

<style lang="scss" scoped>
.cell {
  width: 100%;
  height: 100%;
  display: flex;
  margin-top: 10px;
  padding: 8px;
  position: relative;
  align-items: center;
  &:after {
    content: "";
    position: absolute;
    right: 0;
    left: 0;
    bottom: 0;
    border-bottom: 1px solid rgb(218, 180, 180);
    transform: scaleY(0.5);
    z-index: 2;
  }
  p {
    flex: 1;
    font-size: 14px;
  }
  .value{
      text-align: right;
  }
}
</style>