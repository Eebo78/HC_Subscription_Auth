// module.exports = {
//   chainWebpack: (config) => {
//     //graphql loader
//     config.module
//       .rule("graphql")
//       .test(/\.graphql$/)
//       .use("graphql-tag/loader")
//       .loader("graphql-tag/loader")
//       .end();
//   },
// };
// vue.config.js
module.exports = {
  chainWebpack: (config) => {
    // GraphQL Loader
    config.module
      .rule("graphql")
      .test(/\.(graphql|gql)$/)
      .use("graphql-tag/loader")
      .loader("graphql-tag/loader")
      .end();
    // Add another loader
    // .use('other-loader')
    //   .loader('other-loader')
    //   .end()
  },
};
