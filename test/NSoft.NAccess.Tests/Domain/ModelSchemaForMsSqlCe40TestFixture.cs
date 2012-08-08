namespace NSoft.NAccess.Domain
{
#if TEST_MS_SQL_CE
	[TestFixture]
	public class ModelSchemaForMsSqlCe40TestFixture : DomainTestFixtureBase
	{
		#region << logger >>

		private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

		#endregion

		protected override DatabaseEngine GetDatabaseEngine()
		{
			return DatabaseEngine.MsSqlCe40;
		}

		[Test]
		public void GenerateSchema()
		{
			if(log.IsDebugEnabled)
				log.Debug(@"Generate Schema");

			new SchemaExport(CurrentContext.NHConfiguration).SetDelimiter(@";").Create(true, false);
		}

		[Test]
		public void GenerateSchemaToFile()
		{
			if(log.IsDebugEnabled)
				log.Debug(@"Schema를 생성하고, Oracle DB를 실제로 생성합니다...");

			new SchemaExport(CurrentContext.NHConfiguration)
				.SetOutputFile(@".\..\..\NAccess.MsSqlCe40.sql")
				.SetDelimiter(@";")
				.Create(true, true);

			if(log.IsDebugEnabled)
				log.Debug(@"Schema를 생성하고, DB를 실제로 생성했습니다!!!");
		}
	}
#endif
}