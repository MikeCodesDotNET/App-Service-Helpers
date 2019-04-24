module.exports = {
    title: 'App Service Helpers Docs',
    markdown: {
      lineNumbers: true
    },
    themeConfig: {
      sidebarDepth: 5,
      sidebar: [
        {
          title: 'Getting Started',
          collapsable: false,
          children: [
            '/gettingstarted/Backend',
            '/gettingstarted/Windows',
            '/gettingstarted/macOS',
          ]
        },
        {
          title: 'Data Storage',
          collapsable: false,
          children: [
            '/datastorage/',
            '/datastorage/datamodel',
            '/datastorage/automaticdatastorage',
            '/datastorage/datastoragetables',
          ]
        }
            
      ]
    }

    


}